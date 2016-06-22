$(function () {

    function initializePhotos() {
        var photos = $(".photo-container");

        _.each(photos, function(photo) {
            var id = $(photo).data("id");

            if (!id) return;

            $.ajax({
                type: 'POST',
                url: "/photo/getphoto",
                data: { 'id': id },
                success: function(response) {
                    if (response) {
                        $(photo).html($('<img class="image-medium" />').attr({
                            src: response
                        }));
                    } else {
                        $('#messageModal .text-info').text('Something goes wrong');
                        $('#messageModal').modal('show');
                    }
                },
                error: function(message) {
                    console.log(message);
                }
            });
        });
    };

    initializePhotos();

    $('.datepicker').datepicker({
        formatDate: "mm/dd/yyyy"
    });
    
    $('#addPhotoLink').on('click', function (event) {
        event.preventDefault();
        $('.error-message').empty();
        $('#addPhotoModal').modal('show');
    });

    $('#addPhotoForm').on('submit', function(event) {
        event.preventDefault();
        var $this = $(this);
        var files = $("#ImageToUpload").get(0).files;

        if (!files || files.length <= 0) {
            $('.error-message').text('Image is required');
            return;
        }

        var formData = new FormData();
        formData.append('file', files[0]);

        var imageDetails = $this.serializeArray();
        _.each(imageDetails, function (obj) {
            formData.append(obj.name, obj.value);
        });

        $.ajax({
            type: 'post',
            url: $this.attr('action'),
            processData: false,
            contentType: false,
            data: formData,
            success: function(response) {
                if (response === 'ok') {
                    $('.error-message').empty();
                    $('#addPhotoModal').modal('hide');
                    document.getElementById("addPhotoForm").reset();

                    $('#messageModal .text-info').text('Photo was added successfully');
                    $('#messageModal').modal('show');
                } else {
                    $('.error-message').text(response);
                }
            }
        });
    });

    var photoId = null;
    $('.albumstolink').on('click', function (event) {
        event.preventDefault();
        photoId = $(this).data('id');
        $('#messageModal .text-info').empty();
        $('#messageModal .error-message').empty();
        $('#linkPhotoModal').modal('show');
    });
   
    $('#linkPhotoForm').on('submit', function(event) {
        event.preventDefault();
        var $this = $(this);
        var albumId = $('#AlbumId').val();
        var formData = new FormData();

        formData.append('photoId', photoId);
        formData.append('albumId', albumId);

        $.ajax({
            type: 'post',
            url: $this.attr('action'),
            processData: false,
            contentType: false,
            data: formData,
            success: function(response) {
                if (response === 'ok') {
                    $("#messageModal .text-info").text('Photo was linked successfully');
                    $('#linkPhotoModal').modal('hide');
                } else {
                    $('#messageModal .error-message').text(response);
                }

                $('#messageModal').modal('show');
            }
        });
    });

    $('.make-title-photo').on('click', function(event) {
        event.preventDefault();
        var $this = $(this);
        $("#messageModal .text-info").empty();

        var albumId = $($this).data("album-id");
        var photoId = $($this).data("photo-id");

        if (!albumId || !photoId) {
            $('#messageModal .error-message').text('Sorry, something goes wrong');
            $('#messageModal').modal('show');
            return;
        }

        $.ajax({
            type: 'POST',
            url: $this.attr('href'),
            data: { 'albumId': albumId, 'photoId': photoId },
            success: function(response) {
                if (response === 'ok') {
                    $("#messageModal .text-info").text('Album title was changed successfully');
                } else {
                    $("#messageModal .text-info").empty();
                    $('#messageModal .error-message').text(response);
                }

                $('#messageModal').modal('show');
            }
        });
    });

    $('.remove-photo').on('click', function (event) {
        event.preventDefault();
        if (!confirm('Are you sure?')) return false;

        var $this = $(this);
        $("#messageModal .text-info").empty();
        $('#messageModal .error-message').empty();

        var photoId = $($this).data("id");

        $.ajax({
            type: 'POST',
            url: $this.attr('href'),
            data: { 'id': photoId },
            success: function (response) {
                if (response === 'ok') {
                    $($this).parent().remove();

                    $("#messageModal .text-info").text('Photo was deleted successfully');
                } else {
                    $("#messageModal .text-info").empty();
                    $('#messageModal .error-message').text(response);
                }

                $('#messageModal').modal('show');
            }
        });
    });
});