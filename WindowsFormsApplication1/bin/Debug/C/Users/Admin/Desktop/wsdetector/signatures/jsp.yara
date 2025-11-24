rule JSP_Webshell_XSLT_RCE
{
    meta:
        description = "Detects JSP WebShell using XSLT Transformer for remote code execution"
        author = "Tung Trung"
        type = "webshell"
        date = "2025-11-15"
        score = 90
        id = "jsp-xslt-rce-006"

    strings:
        // Thành phần XSLT Transformer nguy hiểm
        $xslt1 = "TransformerFactory.newInstance"
        $xslt2 = "new StreamSource"
        $xslt3 = "newTransformer"

        // Dấu hiệu của việc nhận XSLT template từ attacker
        $read1 = "request.getReader"
        $read2 = "BufferedReader"

        // Dấu hiệu của WebShell: lấy toàn bộ body vào string
        $loop1 = "while ((line = br.readLine()) != null)"
        $big1  = "ByteArrayOutputStream"
        $big2  = "ByteArrayInputStream"

        // Khả năng trả output cho attacker
        $res1 = "response.getWriter().print"

    condition:
        // Phải có XSLT + đọc body + transform
        all of ($xslt*) and
        all of ($read*) and
        1 of ($loop1, $big1, $big2, $res1)
}
