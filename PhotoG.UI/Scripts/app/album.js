$(function () {
    $(".get-link-button").click(function() {
        var url = $(this).data("url");
        $(".direct-link").text(url);
    });

    function intializeAlbums() {
        var titles = $('.title-container');

        _.each(titles, function(title) {
            var albumId = $(title).data("id");

            $.ajax({
                type: 'POST',
                url: "/album/GetTitlePhoto",
                data: { 'albumId': albumId },
                success: function(response) {
                    if (response) {
                        $(title).html($('<img class="image-small" />').attr({
                            src: response
                        }));
                    } else {
                        $('#messageModal .error-message').text('Something goes wrong');
                        $('#messageModal').modal('show');
                    }
                },
                error: function(message) {
                    console.log(message);
                }
            });
        });
    };

    intializeAlbums();
});