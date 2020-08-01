# requires GitPython
from git import Repo
import json

repo = Repo(r'..\..') # Assuming this is run from same folder as hexinstall.py script, gets the repo from two folders up
submod_basepath = 'Assets/Dependencies/{name}'
submod_baseurl = 'https://github.com/hexthedev/{name}.git'
submod_basename = 'HexUN-{name}'

# Read a json file with hexlibs [string], which contains hexlib names
# Possible libs: UXUI, RTS, AI, Grid, Input
with open('./hexinstall.json') as f:
  data = json.load(f)

  # Add core submodule
  repo.create_submodule('HexUN', submod_basepath.format(name = 'HexUN'), url=submod_baseurl.format(name='HexUN'), branch='master' )

  # Add other submodules
  for module in data['hexlibs']:
    name = submod_basename.format(name=module)
    repo.create_submodule(name, submod_basepath.format(name = name), url=submod_baseurl.format(name=name), branch='master')

  print(data)
