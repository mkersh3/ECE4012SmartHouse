import xml.etree.ElementTree as ET
from models import Sensor
from django import forms
import os

directory = "../SmartHouseSD/Final/SmartHouseSD/SmartHouseSD/bin/Debug/"
filename = "sensor_data.xml"

#class CommandForm(forms.Form):
#    light = forms.BooleanField(required=False)
#    activity = forms.IntegerField(required=False,min_value=0,max_value=3)
#    armed = forms.BooleanField(required=False)


def webToHome():
    #update xml file based on database
    tree = ET.parse(directory + filename)
    root = tree.getroot()
    #perhaps add attribute to Sensor model for "writeable"
    #comnds = ["Light","Activity Level", "Armed"]
    light_key = "L"
    lev_key = "Z"
    
    #print light, activity, armed
    for node in root.iter("sensor"):
        if(node.attrib["key"] == light_key):
            print "changing light"
            light = Sensor.objects.filter(key=light_key)[0]
            node.attrib["value"] = str(light.value)
        elif(node.attrib["key"] == lev_key):
            lev = Sensor.objects.filter(key=lev_key)[0]
            node.attrib["value"] = str(lev.value)
    
    root.attrib["dataFromWebToHome"] = "1"
    print tree.getroot().attrib["dataFromWebToHome"]
    tree.write(directory + filename)
    
    
def homeToWeb():
    tree = ET.parse(directory + filename)
    root = tree.getroot()
    for node in root.iter("sensor"):
        if (Sensor.objects.filter(name=node.attrib["name"])):
            sensor = Sensor.objects.filter(name=node.attrib["name"])
            for s in sensor:
                s.value = node.attrib["value"]
                s = s.save()
        else:
            print "creating sensor in db"
            sensor = Sensor.objects.create(name=node.attrib["name"],value=node.attrib["value"],key=node.attrib["key"])
            sensor = sensor.save()
    root.attrib["dataFromHomeToWeb"] = "0"
    tree.write(directory + filename)
    