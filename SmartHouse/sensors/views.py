from django.shortcuts import render
from django.template import RequestContext, loader
from django.http import HttpResponse, HttpResponseRedirect,HttpResponseNotFound
from django.views.generic.edit import UpdateView

from django import forms

from sensorData import webToHome, homeToWeb#not changed in desktop app on laptop
from models import Sensor, CommandForm

class CommandBothForm(forms.Form):
    light = forms.BooleanField(required=False)
    level = forms.IntegerField(min_value=0,max_value=3)

# Create your views here.
def index(request):
    #build template for homepage. user info here
    template = loader.get_template('index.html')
    bar = "George P. Burdell"
    context = RequestContext(request, {
        'foo':bar,
    })
    return HttpResponse(template.render(context))
    
def status(request):
    homeToWeb()
    sensor_list = Sensor.objects.order_by('name')
    template = loader.get_template('status.html')
    #this is probably the preferred order
    context = RequestContext(request, {
        'sensor_list':sensor_list,
    })
    return HttpResponse(template.render(context))

def light(request):
    #this may have to be extended for all commands.....
    #3 buttons: light(0,1), arm/disarm(0,1),activity level(0,1,2,3)
    #form will be separate page for now
    light_name = "Light"
    light = Sensor.objects.filter(name=light_name)
    light = light[0]
    if request.method == 'POST':
        if light:
            form = CommandForm(request.POST, instance=light)
            if form.is_valid():
                if(form.cleaned_data['value']):
                    light.value = form.cleaned_data['value']
                    light = light.save()
                    webToHome()
                    return HttpResponseRedirect('/command_light/')
                else:
                    return HttpResponseNotFound('<h1>Sensor not found</h1>')
        else:
            return HttpResponseNotFound('<h1>Sensor not found</h1>')
    else:   
        form = CommandForm()
    
    return render(request, 'command_light.html',{
        'form':form,
    })
    
def level(request):
    #this may have to be extended for all commands.....
    #3 buttons: light(0,1), arm/disarm(0,1),activity level(0,1,2,3)
    #form will be separate page for now
    act_key = "Z"
    #arm_name = "Armed"
    lev = Sensor.objects.filter(key=act_key)
    lev = lev[0]
    if request.method == 'POST':
        if lev:
            form = CommandForm(request.POST, instance=lev)
            if form.is_valid():
                if(form.cleaned_data['value']):
                    lev.value = form.cleaned_data['value']
                    lev = lev.save()
                    webToHome()
                    return HttpResponseRedirect('/command_level/')
                else:
                    return HttpResponseNotFound('<h1>Sensor not found</h1>')
        else:
            return HttpResponseNotFound('<h1>Sensor not found</h1>')
    else:   
        form = CommandForm()
    
    return render(request, 'command_level.html',{
        'form':form,
    })
    
def editBoth(request):
    light = Sensor.objects.filter(key="L")[0]
    level = Sensor.objects.filter(key="Z")[0]
    if request.method == 'POST':
        form = CommandBothForm(request.POST)
        if form.is_valid():
            light.value = form.cleaned_data['light']
            level.value = form.cleaned_data['level']
            light = light.save()
            level = level.save()
            webToHome()
            return HttpResponseRedirect('/command_both/')
        else:
            return HttpResponseNotFound('<h1>That form is not valid!</h1>')
    else:   
        form = CommandBothForm()
    
    return render(request, 'command_both.html',{
        'form':form,
    })