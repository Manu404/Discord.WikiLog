#!/bin/bash

projectVersion="1.0.0.0"
projetFile="./DiscordWikiLog/DiscordWikiLog.csproj"
projectName="discordwikilog"
buildScript="multiplateform_build.sh"

chmod 755 $buildScript
cd ../
./build/MultiplateformDotNetCoreBuildScript/$buildScript -p $projetFile -n $projectName -v $projectVersion	
