#!/bin/bash

cd `dirname $0`

UDID_PATTERN="[[:alnum:]]{8}-[[:alnum:]]{4}-[[:alnum:]]{4}-[[:alnum:]]{4}-[[:alnum:]]{12}"

ARGS=($@)

SCRIPT_NAME=`basename $0`

if [ "$1" = "-h" ]; then
  cat << END
iOS Simulator Launcher
	Usage: $SCRIPT_NAME [UDID] [dotnet command options]

If you don't specify UDID, you can select UDID from booting simulator device list.

dotnet command will executed like below ...
	dotnet build -t:Run ./FIS-J/FIS-J.csproj -f net6.0-ios -r iossimulator-x64 --no-self-contained --nologo "/p:_DeviceName=:v2:udid=[UDID]" [dotnet command options]

---

Exec Example: Run Release build on the simulator (UDID=12345678-1234-1234-1234-123456789ABC)
	$SCRIPT_NAME 12345678-1234-1234-1234-123456789ABC -c Release


END

  exit 0
fi

ARG_UDID=`echo "$1" | grep -oE "$UDID_PATTERN"`

if [ -z "$1" -o -z "$ARG_UDID" ]; then
  echo "Cannot detect UDID from command line argument ..." 1>&2

  COUNTER=0
  declare -a DeviceArr=()
  while read ListLine
  do
    if [[ $ListLine == --* ]]; then
      echo "$ListLine"
    else
      echo "[$COUNTER]" "$ListLine"
      DeviceArr[$COUNTER]="$ListLine"
      COUNTER=`expr $COUNTER + 1`
    fi
  done <<< "$(/Applications/Xcode.app/Contents/Developer/usr/bin/simctl list | grep -E '^\-\-|Booted' | grep -v 'Unavailable')"

  ARRLEN=${#DeviceArr[*]}
  if [ $ARRLEN -eq 0 ]; then
    echo "No booting Simulated Device detected.  please launch 'Simulator.app' and open simulator." 1>&2
    exit 0
  fi

  echo "Please select the device you want to launch App on."
  INPUT_MAX=`expr $ARRLEN - 1`; read -p "0 ~ $INPUT_MAX [0]: " answer

  if [ -z "$answer" ]; then
    answer=0
  fi

  if [ ! \( 0 -le $answer -a $answer -lt $ARRLEN \) ]; then
    echo "Invalid Selection ($answer)" 1>&2
    exit 1
  fi

  UDID=`echo ${DeviceArr[$answer]} | grep -o -E "$UDID_PATTERN"`
else
  UDID=$ARG_UDID
  ARGS[0]=""
fi


if [ ! -z "$UDID" ]; then
  dotnet build -t:Run ./FIS-J/FIS-J.csproj -f net6.0-ios -r iossimulator-x64 --self-contained --nologo "/p:_DeviceName=:v2:udid=$UDID" ${ARGS[@]}
fi
