var dataTable;

$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tbldata').DataTable({
        "ajax": {
            "url": "Booking/GetAll",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "name", "width": "auto", "className": "text-center" }, // Center align and let DataTable decide width
            { "data": "address", "width": "auto", "className": "text-center" },
            { "data": "email", "width": "auto", "className": "text-center" },
            { "data": "phoneNumber", "width": "auto", "className": "text-center" },
            { "data": "numberOfPersons", "width": "auto", "className": "text-center" },
            { "data": "ticketPrice", "width": "auto", "className": "text-center" },
            { "data": "bookingStatus", "width": "auto", "className": "text-center" },
            { "data": "count", "width": "auto", "className": "text-center" },
            { "data": "date", "width": "auto", "className": "text-center" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                       <div class="text-center">
                      <a href="Booking/Upsert/${data}" class="btn btn-info">
                      <i class="fas fa-edit"></i>
                      </a>
                     <a class="btn btn-danger" onclick=Delete('Booking/Delete/${data}')>
                       <i class="fas fa-trash-alt"></i>
                       </a>
                       </div>
                          `;
                }
            }
        ]
    })

}
function Delete(url) {
    // alert(url);
    swal({
        title: "Want to delete data?",
        text: "Delete Information!!!",
        icon: "warning",
        buttons: true,
        dangerModel: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })

}
