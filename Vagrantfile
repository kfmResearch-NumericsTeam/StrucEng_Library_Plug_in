Vagrant.configure("2") do |config|

  config.vm.provider "virtualbox"
  config.vm.box = "generic/fedora35"
  config.vm.box_version = "4.0.2"
  config.vm.synced_folder ".", "/vagrant"
  config.vm.provision "shell", path: "./tools/vagrant/setup_fedora_35.sh"
end
