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
                { 'data': 'category.name', "width": "15%" }
            ]
        }
    )
}