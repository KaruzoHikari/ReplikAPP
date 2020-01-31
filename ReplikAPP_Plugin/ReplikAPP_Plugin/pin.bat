@echo off
setlocal
cls

:begin
echo ------------------------------------
set /P "pin=INPUT YOUR PIN: "
echo ------------------------------------

call :isInt %pin% || goto invalid
call :strlen strlen pin

if %strlen% == 6 (
	echo THE PIN IS VALID.
	echo ------------------------------------
	break > %TMP%\UserPin.txt
	@echo %pin%>> %TMP%\UserPin.txt
	timeout 1 > NUL
	exit
) else (
	echo THE PIN NEEDS 6 NUMBERS.
	echo TRY AGAIN.
	echo ------------------------------------
	timeout 2 > NUL
	cls
	goto begin
)



:invalid
echo THE PIN CAN ONLY HAVE NUMBERS.
echo TRY AGAIN.
echo ------------------------------------
timeout 2 > NUL
cls
goto begin

:isInt <str>
for /f "delims=0123456789" %%a in ("%1") do exit /b 1
exit /b 0

:strlen <resultVar> <stringVar>
(   
    setlocal EnableDelayedExpansion
    (set^ tmp=!%~2!)
    if defined tmp (
        set "len=1"
        for %%P in (4096 2048 1024 512 256 128 64 32 16 8 4 2 1) do (
            if "!tmp:~%%P,1!" NEQ "" ( 
                set /a "len+=%%P"
                set "tmp=!tmp:~%%P!"
            )
        )
    ) ELSE (
        set len=0
    )
)
( 
    endlocal
    set "%~1=%len%"
    exit /b
)