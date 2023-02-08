$(document).on('click', '#productLink', function () {
    var productId = $(this).children('#productId').val();
    console.log(productId)
    $.ajax({
        type: "POST",
        url: '/WishList/AddProductToWishList',
        data: { productId: productId },
        success: function () {
            $("#favorite" + productId).removeClass("fa-regular fa-heart");
            $("#favorite" + productId).addClass("fa-solid fa-heart");
        },
    });
})
$(document).on('click', '#bookLink', function () {
    var productId = $(this).children().val();
    console.log(productId)
    $.ajax({
        type: "POST",
        url: "/WishList/DeleteProductFromWishList",
        data: { productId: productId },
        success: function () {
            $("#" + productId + "tr").remove();
            
        },
    });
})