VERSION_DOTS="1.5.8"
VERSION_UNDERSCORES=${VERSION_DOTS//./_}
FILE_NAME="IPBan-Linux-x64_$VERSION_UNDERSCORES.zip"

# run entire script from url, do sudo -i first
# sudo -i;
# bash <(wget -qO- https://raw.githubusercontent.com/DigitalRuby/IPBan/master/IPBanCore/Linux/Scripts/Install.sh)

# install unzipper, install iptables and ipset
sudo apt -q -y install unzip iptables ipset || true; sudo apt -q -y update || true;
sudo yum -q -y install unzip iptables ipset || true; sudo yum -q -y update || true;

# make folder /opt/ipban
sudo mkdir /opt/ipban -p; cd /opt/ipban;

# stop service
sudo systemctl stop ipban

# save off ipban.sqlite and ipban.config files
mkdir /tmp>/dev/null || :
cp /opt/ipban/ipban.sqlite /tmp>/dev/null || :
cp /opt/ipban/ipban.config /tmp>/dev/null || :

# download latest release and extract to /opt/ipban
sudo wget https://github.com/DigitalRuby/IPBan/releases/download/$VERSION_DOTS/$FILE_NAME; sudo unzip -qq $FILE_NAME; sudo rm $FILE_NAME;

# allow execute permissions for /opt/ipban/DigitalRuby.IPBan
sudo chmod +x /opt/ipban/DigitalRuby.IPBan

# install service to run the executable
sudo cat > /lib/systemd/system/ipban.service <<"END.OF.TEMPLATE"

[Unit]
Description=IPBan Service
After=network.target

[Service]
Type=notify
WorkingDirectory=/opt/ipban
ExecStart=/opt/ipban/DigitalRuby.IPBan
Restart=on-failure
KillSignal=SIGINT

[Install]
WantedBy=multi-user.target
END.OF.TEMPLATE

# restore sqlite and config files
cp /tmp/ipban.sqlite /opt/ipban>/dev/null || :
rm /tmp/ipban.sqlite>/dev/null || :
cp /tmp/ipban.config /opt/ipban>/dev/null || :
rm /tmp/ipban.config>/dev/null || :

# enable and start service, ensure that it is running on reboots as well
sudo systemctl daemon-reload; sudo systemctl start ipban; sudo systemctl restart ipban; sudo systemctl enable ipban; sudo systemctl status ipban;

# open up config editor to make any additional changes like whitelist or min failed attempt to ban, etc.
sudo nano /opt/ipban/ipban.config
