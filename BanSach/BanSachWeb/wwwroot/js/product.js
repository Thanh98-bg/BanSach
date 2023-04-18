var dataTable;
$(document).ready(function () {
    loadDatatable();
});

function loadDatatable() {
    dataTable = $('#tblData').dataTable(
        {
            "ajax": {
                "url": "/admin/product/getall"
            },
            "columns": [
                {'data': 'title', "width":"15%"},
                {'data': 'isbn', "width":"15%"},
                {'data': 'price50', "width":"15%"},
                {'data': 'author', "width":"15%"},
                { 'data': 'category.name', "width": "15%" },
                {
                    'data': 'id',
                    'render': function (data) {
                        return `<div>
                        <a href="/Admin/Product/Upsert?id=${data}" class="btn btn-primary" > <i class="bi bi-pencil"></i></a>
                        <a class="btn btn-primary"><i class="bi bi-trash3"></i></a>
                        </div >`
                    },
                    "width": "15%"
                }
            ]
        }
    )
}