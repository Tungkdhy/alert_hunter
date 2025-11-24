rule JSP_Webshell_Base64_Embedded
{
    meta:
        description = "Detects large Base64 payload inside JSP WebShell"
        author = "Tung Trung"
        type = "webshell"
        date = "2025-11-15"
        score = 75
        id = "jsp-base64-003"

    strings:
        $b64 = /[A-Za-z0-9+\/=]{1500,}/
        $tmp = "FileOutputStream"
        $dec1 = "Base64"
        $dec2 = "decode"

    condition:
        $b64 and 1 of ($tmp, $dec1, $dec2)
}
