Vagrant.configure("2") do |config|

  config.vm.provider "virtualbox"
  config.vm.box = "bento/ubuntu-18.04"
  config.vm.box_version = "202112.19.0"
  config.vm.synced_folder ".", "/vagrant"
  config.vm.provision "shell", inline: <<-SHELL
          /vagrant/tools/vagrant/setup_ubuntu_18_04.sh
  SHELL
end
