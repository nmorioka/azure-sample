/**
* 指定されたファイルを画面に表示する。
* @param    {FileList}  files   読み取るファイルリスト
*/
var read = function (files) {
    var fragment, i, item;

    fragment = document.createDocumentFragment();

    for (i = files.length; i--;) {
        item = document.createElement('div');
        item.appendChild(document.createTextNode(files[i].name));
        fragment.appendChild(item);
    }

    $('#msg').append(fragment);
};

/**
* dragover イベント が発生したとき呼び出されます。
* @param    {Event}     event   イベントオブジェクト
*/
var body_ondragover = function (event) {
    event.preventDefault();
    $('#msg').text('ondragover');
};

/**
* drop イベント が発生したとき呼び出されます。
* @param    {Event}     event   イベントオブジェクト
*/
var body_ondrop = function (event) {
    var i, files, fragment, item;

    $('#msg').text('ondrop');

    files = event.originalEvent.dataTransfer.files || [];

    read(files);
    upload(files);
};

/**
* ドキュメント生成が完了したとき呼び出されます。
*/
var document_onready = function (event) {
    console.log("hogehogefugafuga")
    $(window.document.body).on(
        'dragover', body_ondragover
    ).on(
        'drop', body_ondrop
    );

    $('#submitBtn').on('click', function (evt) {
        var form = $('#myForm').get()[0];
        var formData = new FormData(form);

        $.ajax({
            url: '/api/file',
            method: 'POST',
            processData: false,
            contentType: false,
            data: formData
        }).done(function (data, textStatus, jqXHR) {
            console.log("success..");
            $('#msg').append(JSON.stringify(data));
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log("fail..");
            $('#msg').append(textStatus);
        });
    });

    $('#submitBtn2').on('click', function (evt) {

        var data = {};
        data.imageId = $("#imageId").val();

        console.log("p : " +data);

        $.ajax({
            url: '/api/processing',
            method: 'POST',
            dataType: 'json',
            data: data
        }).done(function (data, textStatus, jqXHR) {
            console.log("success..");
            $('#msg').append(JSON.stringify(data));
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.log("fail..");
            $('#msg').append(textStatus);
        });
    });
};


$(document).ready(document_onready);