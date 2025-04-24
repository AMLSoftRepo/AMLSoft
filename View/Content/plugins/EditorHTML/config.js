/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    //config.toolbarGroups = [
    //        { name: 'document', groups: ['mode', 'document', 'doctools'] },
    //        { name: 'clipboard', groups: ['clipboard', 'undo'] },
    //        { name: 'editing', groups: ['find', 'selection', 'spellchecker', 'editing'] },
    //        { name: 'forms', groups: ['forms'] },
    //        '/',
    //        { name: 'basicstyles', groups: ['basicstyles', 'cleanup'] },
    //        { name: 'paragraph', groups: ['list', 'indent', 'blocks', 'align', 'bidi', 'paragraph'] },
    //        { name: 'links', groups: ['links'] },
    //        { name: 'insert', groups: ['insert'] },
    //        '/',
    //        { name: 'styles', groups: ['styles'] },
    //        { name: 'colors', groups: ['colors'] },
    //        { name: 'tools', groups: ['tools'] },
    //        { name: 'others', groups: ['others'] },
    //        { name: 'about', groups: ['about'] }
    //];

    //config.removeButtons = 'Source,NewPage,Preview,Templates,Iframe,Smiley,Flash,ShowBlocks,About,Find,Replace,Scayt,CreateDiv,Language,Save';

    //config.removePlugins = 'elementspath';



    config.toolbar = 'Custom';

    config.toolbar_Custom = [
        ['Print', '-'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-'],
        ['Undo', 'Redo'], ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],          
        ['Font', 'FontSize', 'TextColor', 'BGColor'],
        ['Bold', 'Italic', 'Underline','-'], ['Maximize']
    ];

    config.removePlugins = 'elementspath';
    config.language = 'es';
    
};

