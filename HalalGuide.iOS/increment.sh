#!/bin/bash
bN=$(/usr/libexec/PlistBuddy -c "Print :CFBundleVersion" "Info.plist")
bN=$(($bN))
bN=$(($bN + 1))
bN=$(printf "%d" $bN)
/usr/libexec/PlistBuddy -c "Set :CFBundleVersion $bN" "Info.plist"