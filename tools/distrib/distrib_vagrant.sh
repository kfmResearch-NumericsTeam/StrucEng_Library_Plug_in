#!/usr/bin/env bash
set -euo pipefail

set -x
vagrant ssh -c "cd /vagrant/tools/distrib/; ./distrib.sh $@"
set +x
