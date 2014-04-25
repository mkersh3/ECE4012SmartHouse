from django.conf.urls import patterns, include, url

from django.contrib import admin
admin.autodiscover()

from sensors import views
#from sensors.views import CommandForm

urlpatterns = patterns('',
    # Examples:
    # url(r'^$', 'SmartHouse.views.home', name='home'),
    # url(r'^blog/', include('blog.urls')),

    url(r'^admin/', include(admin.site.urls)),
    url(r'^$', views.index, name='index'),
    url(r'^status/', views.status, name='status'),
    url(r'^command_light/',views.light, name='command_light'),
    url(r'^command_level/',views.level,name='command_level'),
    url(r'^command_both/', views.editBoth, name='command_both'),
)
