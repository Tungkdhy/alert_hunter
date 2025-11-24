rule JSP_Webshell_XMLDecoder_RCE
{
    meta:
        description = "Detect JSP WebShell using java.beans.XMLDecoder to deserialize XML payload from request parameter"
        author = "Trung Tung"
        date = "2025-11-15"
        type = "webshell"
        score = 80
        id = "jsp-xmldecoder-rce-009"

    strings:
        $decoder = "java.beans.XMLDecoder"
        $ba_in   = "new ByteArrayInputStream"
        $bx_in   = "new BufferedInputStream"
        $param   = "request.getParameter(\"ladypwd\")"

    condition:
        $decoder and $ba_in and $bx_in and $param
}
