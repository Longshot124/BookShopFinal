$(document).on('click', '#productLink', function () {
    var productId = $(this).parent().parent().parent().parent().children().val();
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
$(document).on('click', '#add-product-to-cart', function (e) {
    var bookId = $(this).parent().parent().parent().parent().children().val();
    $.ajax({
        type: "POST",
        url: '/Basket/AddToBasket',
        data: { bookId: bookId },
        success: function () {
        },
    });
})

$(document).on('click', '#remove-from-cart', function () {
    var bookId = $(this).parent().parent().children().val();
    console.log(bookId)
    $.ajax({
        type: "POST",
        url: "/Basket/DeleteProductBasket",
        data: { bookId: bookId },
        success: function () {
            console.log("hello");
            $("#" + bookId + "tr").remove();
        },
        Error: function () {
            console.log("good bye")
            alert("Somthing Wrong");
        }
    });

});

$(document).on('change', '#product-quality', function () {
    var bookId = $(this).parent().parent().parent().children().val();
    console.log(bookId)
    var count = $(this).val();
    var price = $("#product-price").attr("data-id");
    var totalPrice = price * count;

    $.ajax({
        type: "POST",
        url: "/Basket/ChangeProductQuality",
        data: { bookId: bookId, count: count },
        success: function () {
            $("#" + "product-total-price" + bookId).html("$"+(totalPrice));
        },
        Error: function () {
            alert("Somthing Wrong");
        }
    });



});