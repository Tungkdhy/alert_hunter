rule JSP_Webshell_Statement_Invoke_RCE
{
    meta:
        description = "Detects JSP WebShell using java.beans.Statement + reflection to execute commands"
        author = "Tung Trung"
        date = "2025-11-15"
        score = 70

    strings:
        $statement   = "java.beans.Statement"
        $new_stmt    = "new Statement"
        $exec_split  = "e\"+\"x\"+\"e\"+\"c"       // exec bị tách nhỏ
        $invoke_mtd  = "getDeclaredMethod(\"invoke\"" 
        $invoke_call = "(Process) m.invoke"
        $accessible  = "m.setAccessible(true)"
        

    condition:
        // Phải có đủ các dấu hiệu: Statement + exec obfuscation + reflection
        $statement and 
        $new_stmt and 
        $exec_split and 
        $invoke_mtd and 
        $invoke_call and 
        $accessible
}
