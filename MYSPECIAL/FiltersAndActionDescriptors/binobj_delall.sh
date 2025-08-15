#!/bin/bash

# This file will PULL all git repos in this folder and all subfolders
# MOVE/COPY this file up one directory (to Enterprise405)
# RUN with ". pullallgit.sh" OR "./pullallgit.sh"
find . -iname "bin" | xargs rm -rf
find . -iname "obj" | xargs rm -rf
#find . -iname ".vs" | xargs rm -rf

# oneline
# find . -iname "bin" -o -iname "obj" | xargs rm -rf

# if spaces
# find . -iname "bin" -print0 | xargs -0 rm -rf
# find . -iname "obj" -print0 | xargs -0 rm -rf
# oneline
# find . -iname "bin" -o -iname "obj" -print0 | xargs -0 rm -rf
