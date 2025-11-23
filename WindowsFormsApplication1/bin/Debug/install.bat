@echo off
setlocal

REM ================================
REM  Cấu hình cơ bản
REM ================================
set "SERVICE_NAME=wsdetector"
set "SERVICE_DISPLAY_NAME=WebShell Detector Agent"
set "DEFAULT_INSTALL_DIR=C:\Users\Admin\Desktop\wsdetector"

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
REM  Kiem tra NSSM
REM ================================
set "SCRIPT_DIR=%~dp0"
set "NSSM_EXE=%SCRIPT_DIR%nssm.exe"

if not exist "%NSSM_EXE%" (
    echo [ERROR] Khong tim thay nssm.exe tai:
    echo   "%NSSM_EXE%"
    echo Vui long dat nssm.exe cung thu muc voi install.bat roi chay lai.
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

echo ----------------------------------------------
echo Cai dat wsdetector vao: "%INSTALL_DIR%"
echo ----------------------------------------------

REM Tạo thư mục cài đặt (nếu chưa có)
if not exist "%INSTALL_DIR%" (
    mkdir "%INSTALL_DIR%"
    if %errorlevel% neq 0 (
        echo [ERROR] Khong tao duoc thu muc "%INSTALL_DIR%".
        exit /b 1
    )
)

REM ================================
REM  Copy file
REM ================================
echo [INFO] Thu muc script: "%SCRIPT_DIR%"

echo [INFO] Copy wsdetector.exe ...
copy /Y "%SCRIPT_DIR%wsdetector.exe" "%INSTALL_DIR%" >nul
if %errorlevel% neq 0 (
    echo [ERROR] Khong copy duoc wsdetector.exe
    exit /b 1
)

echo [INFO] Copy config.dat ...
if exist "%SCRIPT_DIR%config.dat" (
    copy /Y "%SCRIPT_DIR%config.dat" "%INSTALL_DIR%" >nul
) else (
    echo [WARN] Khong tim thay config.dat o "%SCRIPT_DIR%"
)

echo [INFO] Copy libcrypto-3-x64.dll ...
if exist "%SCRIPT_DIR%libcrypto-3-x64.dll" (
    copy /Y "%SCRIPT_DIR%libcrypto-3-x64.dll" "%INSTALL_DIR%" >nul
)

echo [INFO] Copy libssl-3-x64.dll ...
if exist "%SCRIPT_DIR%libssl-3-x64.dll" (
    copy /Y "%SCRIPT_DIR%libssl-3-x64.dll" "%INSTALL_DIR%" >nul
)

echo [INFO] Copy thu muc signatures ...
if exist "%SCRIPT_DIR%signatures" (
    where robocopy >nul 2>&1
    if %errorlevel%==0 (
        robocopy "%SCRIPT_DIR%signatures" "%INSTALL_DIR%\signatures" /MIR >nul
    ) else (
        xcopy "%SCRIPT_DIR%signatures" "%INSTALL_DIR%\signatures" /E /I /Y >nul
    )
) else (
    echo [WARN] Khong tim thay thu muc signatures o "%SCRIPT_DIR%"
)

REM ================================
REM  TẠM DỪNG CHO NGƯỜI DÙNG CẤU HÌNH CONFIG.DAT
REM ================================
echo.
echo [CONFIG] File cau hinh hien dang o:
echo   "%INSTALL_DIR%\config.dat"
echo.
echo Ban co the:
echo   - Sua / thay the config.dat trong thu muc tren
echo   - Hoac tao file moi tu config.template (neu co)
echo Sau khi cau hinh xong, nhan phim bat ky de tiep tuc tao service...
echo.
pause

if not exist "%INSTALL_DIR%\config.dat" (
    echo [WARN] Khong tim thay "%INSTALL_DIR%\config.dat".
    echo Service van se duoc tao, nhung co the loi khi chay neu thieu config.dat hop le.
    echo Nhan phim bat ky de tiep tuc...
    pause
)

REM ================================
REM  Xoa service cu (neu co) bang NSSM
REM ================================
echo [INFO] Kiem tra service cu "%SERVICE_NAME%" ...

"%NSSM_EXE%" stop "%SERVICE_NAME%" >nul 2>&1
"%NSSM_EXE%" remove "%SERVICE_NAME%" confirm >nul 2>&1

REM ================================
REM  Tao service moi bang NSSM
REM ================================
echo [INFO] Tao service moi "%SERVICE_NAME%" bang NSSM ...

REM Chuong trinh + tham so
set "APP_EXE=%INSTALL_DIR%\wsdetector.exe"
set "APP_ARGS=-config %INSTALL_DIR%\config.dat"

REM Cai dat service
"%NSSM_EXE%" install "%SERVICE_NAME%" "%APP_EXE%" %APP_ARGS%
if %errorlevel% neq 0 (
    echo [ERROR] NSSM install service that bai.
    pause
    exit /b 1
)

REM Dat working directory de no tim signatures dung thu muc cai dat
"%NSSM_EXE%" set "%SERVICE_NAME%" AppDirectory "%INSTALL_DIR%"
REM Dat display name, start mode
"%NSSM_EXE%" set "%SERVICE_NAME%" DisplayName "%SERVICE_DISPLAY_NAME%"
"%NSSM_EXE%" set "%SERVICE_NAME%" Start SERVICE_AUTO_START

REM Tao thu muc log trong Public
if not exist "%PUBLIC%\wsdetector" mkdir "%PUBLIC%\wsdetector"

"%NSSM_EXE%" set "%SERVICE_NAME%" AppStdout "%PUBLIC%\wsdetector\wsdetector.out.log"
"%NSSM_EXE%" set "%SERVICE_NAME%" AppStderr "%PUBLIC%\wsdetector\wsdetector.err.log"
"%NSSM_EXE%" set "%SERVICE_NAME%" AppRotateFiles 1

REM (Optional) Dat log ra file
REM "%NSSM_EXE%" set "%SERVICE_NAME%" AppStdout "%INSTALL_DIR%\wsdetector.out.log"
REM "%NSSM_EXE%" set "%SERVICE_NAME%" AppStderr "%INSTALL_DIR%\wsdetector.err.log"
REM "%NSSM_EXE%" set "%SERVICE_NAME%" AppRotateFiles 1

REM ================================
REM  Start service
REM ================================
echo [INFO] Start service "%SERVICE_NAME%" ...
"%NSSM_EXE%" start "%SERVICE_NAME%"

echo.
echo [DONE] Cai dat hoan tat.
echo  - Thu muc: %INSTALL_DIR%
echo  - Service: %SERVICE_NAME%  (start=auto, dung NSSM)
echo.
echo Kiem tra trang thai service bang:
echo   nssm status %SERVICE_NAME%
echo Hoac mo Services.msc tim "%SERVICE_DISPLAY_NAME%".
echo.
pause

endlocal
exit /b 0
