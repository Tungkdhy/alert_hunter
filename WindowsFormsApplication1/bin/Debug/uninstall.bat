@echo off
setlocal

REM ================================
REM  Cấu hình cơ bản
REM ================================
set "SERVICE_NAME=wsdetector"
set "DEFAULT_INSTALL_DIR=C:\wsdetector"

REM ================================
REM  Kiểm tra quyền Administrator
REM ================================
>nul 2>&1 net session
if %errorlevel% neq 0 (
    echo [ERROR] Vui long chay script voi quyen Administrator.
    echo  - Right-click -> "Run as administrator"
    echo  - Hoac mo cmd/PowerShell Admin roi chay script nay.
    pause
    exit /b 1
)

REM ================================
REM  Xác định đường dẫn nssm.exe
REM ================================
set "SCRIPT_DIR=%~dp0"
set "NSSM_EXE=%SCRIPT_DIR%nssm.exe"

if not exist "%NSSM_EXE%" (
    echo [ERROR] Khong tim thay nssm.exe tai:
    echo   "%NSSM_EXE%"
    echo Vui long dat nssm.exe cung thu muc voi uninstall.bat.
    pause
    exit /b 1
)

REM ================================
REM  Xác định thư mục cài đặt
REM ================================
if "%~1"=="" (
    set "INSTALL_DIR=%DEFAULT_INSTALL_DIR%"
) else (
    set "INSTALL_DIR=%~1"
)

echo [INFO] Stop + remove service "%SERVICE_NAME%" ...

"%NSSM_EXE%" stop "%SERVICE_NAME%" >nul 2>&1
"%NSSM_EXE%" remove "%SERVICE_NAME%" confirm >nul 2>&1

echo.
if exist "%INSTALL_DIR%" (
    echo [INFO] Dang xoa thu muc cai dat "%INSTALL_DIR%" ...
    rmdir /S /Q "%INSTALL_DIR%"
    if %errorlevel%==0 (
        echo [INFO] Da xoa thu muc cai dat thanh cong.
    ) else (
        echo [WARN] Khong the xoa "%INSTALL_DIR%". Kiem tra quyen hoac file dang bi lock.
    )
) else (
    echo [INFO] Khong tim thay thu muc cai dat "%INSTALL_DIR%". Khong can xoa.
)

echo.
echo [DONE] Go cai dat hoan tat.
echo  - Service "%SERVICE_NAME%" da duoc go bo (neu co).
echo  - Thu muc "%INSTALL_DIR%" da duoc xoa (neu co).
echo.
pause

endlocal
exit /b 0
