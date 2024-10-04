# ADB over WI-FI
To debug the behaviour of your application while an accessory is plugged, you can't use ADB through a USB cable.<br>
In this case, to debug applications, you need to link your device to ADB over Wi-Fi.

## Steps to configure the ADB connection over Wi-Fi

 - Connect the device and the computer to the same Wi-Fi network
 - Plug the device to the computer with a USB cable to configure the connection
 - On the computer command line type: adb tcpip 5555
 - On the computer command line type: adb shell ip addr show wlan0 and copy the IP address after the "inet" until the "/".<br> 
	You can also go inside the Settings of the device to retrieve the IP address in Settings → About → Status. 
 - On the computer command line type: adb connect ip-address-of-device:5555 
 - You can disconnect the USB cable from the device and check with adb devices that the device is still detected.