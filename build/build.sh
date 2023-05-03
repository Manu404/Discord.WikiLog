#!/bin/bash

projectVersion="1.0.0.0"
projetFile="./WikiDiscordNotifier/WikiDiscordNotifier.csproj"
projectName="wikidiscordnotifier"
buildScript="multiplateform_build.sh"

chmod 755 $buildScript
cd ../
./build/MultiplateformDotNetCoreBuildScript/$buildScript -p $projetFile -n $projectName -v $projectVersion	
