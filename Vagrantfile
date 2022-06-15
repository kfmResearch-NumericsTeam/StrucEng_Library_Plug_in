Vagrant.configure("2") do |config|

  config.vm.provider "virtualbox"
  config.vm.box = "bento/ubuntu-20.04"
  config.vm.box_version = "202206.03.0"
  config.vm.synced_folder ".", "/vagrant"
  
#   config.vm.provision "shell", path: "./tools/vagrant/setup_fedora.sh"
end
