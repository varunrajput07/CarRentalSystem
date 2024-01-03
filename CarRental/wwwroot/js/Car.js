var db

$(document).ready(function () {

    localdb();
});

function localdb() {

    db = $('#tbldb').db({

        "ajax": {
            "url":"/Admin/Car/GetAll"
        },
        "Columns": [
            { "data": "Id", "width": "15%" },
            { "data": "Name", "width": "15%" },
            { "data": "Discription", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                        <a href="/Admin/Car/Upsert?id=${data}"
                        class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>
                        <a onClick=Delete('/Admin/Car/Delete/${data}')
                        class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
					</div>
                        `
                },
        ]
    });
//