Vagrant.configure("2") do |config|

  config.vm.provider "virtualbox"
  config.vm.box = "generic/fedora35"
  config.vm.synced_folder ".", "/vagrant"
  config.vm.provision "shell", path: "./tools/vagrant/setup_fedora.sh"
end
