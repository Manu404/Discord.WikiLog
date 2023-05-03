#!/bin/bash

DOTNET_BUNDLE_EXTRACT_BASE_DIR=/var/tmp
PATH=/usr/local/sbin:/usr/local/bin:/sbin:/bin:/usr/sbin:/usr/bin
export DISPLAY=:0.0

cd <installation folder>

./discordwikilog --webhook <webhook_url> --wiki <wiki_url> --api <api_url>
