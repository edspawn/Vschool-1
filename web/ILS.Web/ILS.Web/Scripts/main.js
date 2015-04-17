﻿//документация: docs.sencha.com/ext-js/4-0/

//main.js отвечает за общую часть: верхняя полоска с названием и приветствием пользователя плюс большие кнопки навигации
//в vpConfig мы зададим разметку, которую положим в Ext.container.Viewport - самый внешний контейнер, очерчивающий всю доступную область в браузере
//layout: 'border' распределяет элементы по "регионам" экрана: север-юг-запад-восток-центр
//у нас будут север и центр
//на севере будут dockedItems (появляются на самом верху контейнера) - там верхняя полоска-тулбарчик
//и обычные items - там кнопки навигации
//в центре будет свободная область, в которой другие страницы нарисуют все, что им нужно

var vpConfig = {
    layout: 'border',
    items: [{
        id: 'topToolbarPanel',
        border: false,
        //height: '100%',
        region: 'north',
        xtype: 'panel', //вообще xtype = 'panel' по умолчанию, но для прозрачности решил написать
        cls: 'my-toolbar-panel',
        tools: [{
            xtype: 'button', cls: 'tool-button', text: 'Русский', iconCls: 'russian',
            handler: function () {
                Ext.util.Cookies.set("language", "Russian");
                window.location.href = window.location.href;
            }
        }, {
            xtype: 'button', cls: 'tool-button', text: 'English', iconCls: 'english',
            handler: function () {
                Ext.util.Cookies.set("language", "English");
                window.location.href = window.location.href;
            }
        }, {
            xtype: 'button', cls: 'tool-button', id: 'logButton'
        }, {
            xtype: 'button', cls: 'tool-button', id: 'regButton', handler: function () { window.location.href = link_register; }
        }, {
            xtype: 'button', cls: 'tool-button', id: 'openidButton', handler: function () { window.location.href = link_openid; }
        }],
        items: [{
            xtype: 'panel',
            border: false,
            style: 'background-color: transparent;',
            items: [/*{
                xtype: 'button', scale: 'large', id: 'btn1',                
                iconAlign: 'top', iconCls: 'main_ideology',
                margin: 10,
                handler: function () {
                    window.location.href = link_ideology;
                }
            },*/ {
                xtype: 'button', scale: 'large', id: 'btn2',
                iconAlign: 'top', iconCls: 'main_render',
                width: 150,
                frame: false,
                handler: function () {
                    window.location.href = link_render;
                }
            }, {
                xtype: 'button', scale: 'large', id: 'btn3',
                iconAlign: 'top',
                //здесь и далее иконки определяются через классы CSS, заданные в /Content/icons.css
                iconCls: 'main_struct',
                width: 150,
                handler: function () { //обработчик клика по кнопке
                    //переход по ссылке, определенной в _Layout.cshtml
                    window.location.href = link_struct;
                    //эта ссылка ведет на метод Index контроллера Struct
                    //который возвращает вьюшку Struct/Index.cshtml
                    //которая подключает скрипт struct.js
                    //который нарисует нам редактор курсов, который мы и возжелали, кликнув по кнопке
                }
            }, {
                //    xtype: 'button', scale: 'large', id: 'btn4',
                //    iconAlign: 'top',
                //    iconCls: 'main_flowchart',
                //    margin: 10, width: 150,
                //    handler: function () {
                //        window.location.href = link_flowchart;
                //    }
                //}, {
                xtype: 'button',
                scale: 'large',
                id: 'btn5',
                iconAlign: 'top',
                iconCls: 'main_users',
                width: 150,
                handler: function () {
                    window.location.href = link_users;
                }
            }, {
                xtype: 'button',
                scale: 'large',
                id: 'btn6',
                iconAlign: 'top',
                iconCls: 'main_linkeditor',
                width: 150,
                handler: function () {
                    window.location.href = link_linkeditor;
                }
            }, {
                xtype: 'button',
                scale: 'large',
                id: 'btn7',
                iconAlign: 'top',
                iconCls: 'main_aboutadmin',
                width: 150,
                handler: function () {
                    window.location.href = link_about_admin;
                }
            }, {
                xtype: 'button',
                scale: 'large',
                id: 'btn_testgenerator',
                iconAlign: 'top',
                iconCls: 'main_testgenerator',
                width: 150,
                paddingBottom: 8,
                handler: function () {
                    window.location.href = link_testgenerator;
                }
            }]
        }]
    }, {
        region: 'center',
        xtype: 'panel',
        border: false,
        id: 'mainArea',
        layout: 'fit'
    }]
};

//эту функцию будут вызывать другие скрипты, чтобы заполнить заготовленный центр своими элементами
renderToMainArea = function (cmp) {
    Ext.getCmp('mainArea').add(cmp);
}

var isRussian;

Ext.onReady(function () {
    //console.log(ifLogged.toLowerCase()); console.log(username);
    if (Ext.util.Cookies.get("language") == null) Ext.util.Cookies.set("language", lang_pref);
    if (Ext.util.Cookies.get("language") == "Russian") isRussian = true; else isRussian = false;

    Ext.create('Ext.container.Viewport', vpConfig);

    document.title = '3Ducation';
    if (isRussian) {
        Ext.getCmp('topToolbarPanel').setTitle('3Ducation | Дистанционная обучающая система на основе виртуальных миров');
        //Ext.getCmp('btn1').setText('Наша идеология');
        Ext.getCmp('btn2').setText('Виртуальный мир');
        if ((ifTeacher.toLowerCase() == 'true') || (ifAdmin.toLowerCase() == 'true')) {
            Ext.getCmp('btn3').setText('Редактор курсов');
            //Ext.getCmp('btn4').setText('Диаграмма выполнения');
            Ext.getCmp('btn6').setText('Редактор связей');
        } else {
            Ext.getCmp('btn3').getEl().hide();
            //Ext.getCmp('btn4').getEl().hide();
            Ext.getCmp('btn6').getEl().hide();
        }
        if (ifAdmin.toLowerCase() == 'true') {
            Ext.getCmp('btn5').setText('Пользователи');
            Ext.getCmp('btn7').setText('О проекте');
        } else {
            Ext.getCmp('btn5').getEl().hide();
            Ext.getCmp('btn7').getEl().hide();
        }
        Ext.getCmp('regButton').setText('Регистрация');

        //Ext.getCmp('profileButton').setText('Профиль');
        Ext.getCmp('btn_testgenerator').setText('Генератор тестов');
    } else {
        Ext.getCmp('topToolbarPanel').setTitle('3Ducation | Distance Education System based on Virtual Worlds');
        //Ext.getCmp('btn1').setText('Our Ideology');
        Ext.getCmp('btn2').setText('Virtual World');
        if ((ifTeacher.toLowerCase() == 'true') || (ifAdmin.toLowerCase() == 'true')) {
            Ext.getCmp('btn3').setText('Course Editor');
            //Ext.getCmp('btn4').setText('Workflow');
            Ext.getCmp('btn6').setText('Link Editor');
        } else {
            Ext.getCmp('btn3').getEl().hide();
            //Ext.getCmp('btn4').getEl().hide();
            Ext.getCmp('btn6').getEl().hide();
        }
        if (ifAdmin.toLowerCase() == 'true') {
            Ext.getCmp('btn5').setText('Users');
            Ext.getCmp('btn7').setText('About');
        } else {
            Ext.getCmp('btn5').getEl().hide();
            Ext.getCmp('btn7').getEl().hide();
        }
        Ext.getCmp('regButton').setText('Register');
        //Ext.getCmp('profileButton').setText('Profile');
        Ext.getCmp('btn_testgenerator').setText('Test generator');
    }
    Ext.getCmp('openidButton').setText('OpenID');

    var lb = Ext.getCmp('logButton');
    if ((ifLogged.toLowerCase() == 'false') && isRussian) {
        lb.setIconCls('login2');
        lb.setText('Войти');
        lb.setHandler(function () { window.location.href = link_login; });
        Ext.getCmp('regButton').setVisible(true); //Ext.getCmp('profileButton').setVisible(false);
        Ext.getCmp('openidButton').setVisible(true);
    } else if ((ifLogged.toLowerCase() == 'false') && !isRussian) {
        lb.setIconCls('login2');
        lb.setText('Log on');
        lb.setHandler(function () { window.location.href = link_login; });
        Ext.getCmp('regButton').setVisible(true); //Ext.getCmp('profileButton').setVisible(false);
        Ext.getCmp('openidButton').setVisible(true);
    } else if ((ifLogged.toLowerCase() == 'true') && isRussian) {
        lb.setIconCls('logout2');
        lb.setText('Выйти');
        lb.setHandler(function () { window.location.href = link_logoff; });
        Ext.getCmp('regButton').setVisible(false); //Ext.getCmp('profileButton').setVisible(true);
        Ext.getCmp('openidButton').setVisible(false);
    } else {
        lb.setIconCls('logout2');
        lb.setText('Log off');
        lb.setHandler(function () { window.location.href = link_logoff; });
        Ext.getCmp('regButton').setVisible(false); //Ext.getCmp('profileButton').setVisible(true);
        Ext.getCmp('openidButton').setVisible(false);
    }
});