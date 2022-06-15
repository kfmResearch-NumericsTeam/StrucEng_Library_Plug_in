Vagrant.configure("2") do |config|

  config.vm.provider "virtualbox"
  config.vm.box = "bento/ubuntu-18.04"
  config.vm.synced_folder ".", "/vagrant"
  config.vm.provision "shell", path: "./tools/vagrant/setup_ubuntu_18_04.sh"
end
