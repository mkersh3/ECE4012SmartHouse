from django.db import models
from django import forms


# Create your models here.

class Sensor(models.Model):
    name    = models.CharField(max_length=20)#(ex. Temp1)
    key    = models.CharField(max_length=2)#(ex. Temp, Light, Sound, etc.)
    value   = models.IntegerField(default=0)
        
class CommandForm(forms.ModelForm):
    class Meta:
        model = Sensor
        fields = ["value"]