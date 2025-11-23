import sys
from hashlib import sha256

from Crypto.Cipher import AES
from Crypto.Random import get_random_bytes
import psutil  # yêu cầu phải có


def normalize_mac_for_key(s: str) -> str:
    s = s.lower()
    return ''.join(c for c in s if ('0' <= c <= '9') or ('a' <= c <= 'f'))


def get_mac_for_key() -> str:
    stats = psutil.net_if_stats()
    addrs = psutil.net_if_addrs()
    macs = []
    af_link = getattr(psutil, "AF_LINK", None)

    for name, alist in addrs.items():
        st = stats.get(name)
        if not st or not st.isup:
            continue
        for addr in alist:
            if af_link is not None and addr.family != af_link:
                continue
            mac = normalize_mac_for_key(addr.address)
            if mac and mac != "000000000000":
                macs.append(mac)

    if not macs:
        raise RuntimeError("Không tìm được MAC nào dùng cho key (psutil).")

    max_mac = max(macs)
    if len(max_mac) > 6:
        max_mac = max_mac[-6:]
    return max_mac


def get_config_key(key_suffix: str) -> bytes:
    mac_part = get_mac_for_key()
    print(f"Final MAC for key: {mac_part}")
    key_material = (key_suffix + mac_part).encode("utf-8")
    return sha256(key_material).digest()


def perform_encryption(key, template_path, output_path):
    with open(template_path, 'rb') as f:
        plaintext = f.read()

    nonce = get_random_bytes(12)
    cipher = AES.new(key, AES.MODE_GCM, nonce=nonce)
    ciphertext, tag = cipher.encrypt_and_digest(plaintext)

    encrypted_data = nonce + ciphertext + tag

    with open(output_path, 'wb') as f:
        f.write(encrypted_data)

    print(f"Successfully encrypted '{template_path}' to '{output_path}'.")
    return plaintext


def verify_encryption(key, encrypted_path, original_plaintext):
    print(f"Verifying the encrypted file '{encrypted_path}'...")
    with open(encrypted_path, 'rb') as f:
        encrypted_data_read = f.read()

    nonce_read = encrypted_data_read[:12]
    ciphertext_read = encrypted_data_read[12:-16]
    tag_read = encrypted_data_read[-16:]

    decipher = AES.new(key, AES.MODE_GCM, nonce=nonce_read)

    try:
        decrypted_plaintext = decipher.decrypt_and_verify(ciphertext_read, tag_read)
        print("Decryption plaintext :", decrypted_plaintext.decode('utf-8', errors='ignore'))
        if decrypted_plaintext == original_plaintext:
            print("Verification successful: Decrypted data matches original template.")
        else:
            print("Verification FAILED: Decrypted data does not match original template.")
            sys.exit(1)

    except (ValueError, KeyError) as e:
        print(f"Verification FAILED: Could not decrypt or verify the file. Error: {e}")
        sys.exit(1)


if __name__ == "__main__":
    KEY_SUFFIX = "wsdetector V1.1.4 "
    CONFIG_TEMPLATE_PATH = "config.template"
    CONFIG_DAT_PATH = "config.dat"

    try:
        key = get_config_key(KEY_SUFFIX)

        original_plaintext = perform_encryption(key, CONFIG_TEMPLATE_PATH, CONFIG_DAT_PATH)
        verify_encryption(key, CONFIG_DAT_PATH, original_plaintext)

    except Exception as e:
        print(f"An error occurred during the process: {e}")
        sys.exit(1)
