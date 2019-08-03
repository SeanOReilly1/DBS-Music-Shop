function getCookie(cName) {
    var name = cName + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var cookieArr = decodedCookie.split(';');
    for (var i = 0; i < cookieArr.length; i++) {
        var c = cookieArr[i];
        if (c.indexOf(name) !== -1) {
            return c.substring(c.indexOf(name) + cName.length + 1)
        }
    }
    return "";
}

function read() {
    var cartCookieVal = getCookie("cart");
    if (cartCookieVal !== "") {
        var titlesInCart = cartCookieVal.split(':');
        var titleQty = 0;
        for (var i = 0; i < titlesInCart.length; i++) {
            titleQty += parseInt(titlesInCart[i].split('_')[1]);
        }
        if (titleQty > 0) {
            $("#cartQty").html("(" + titleQty + ") - SHOPPING CART");
        }

    }


}

function cookieExists(cName) {
    var cookie = getCartCookie(cName);
    if (cookie !== "") {
        return true;
    }
    else {
        return false;
    }

}

function addToCart(id) {
    var amt = parseInt($("#txtAmmount" + id).val());
    console.log("amt=" + amt);

    if (amt > 0) {
        var cookie = getCookie("cart");

        if (cookie !== "") {
            var date = new Date();

            date.setTime(date.getTime() + (10 * 24 * 60 * 60 * 1000));//cookie lasts 10 days

            var expires = "; expires=" + date.toUTCString();
            var amt = parseInt($("#txtAmmount" + id).val());
            var itemInCart = false;
            var cart = "cart=";
            var itemsInCart = cookie.split(':');
            var newItemsInCart = new Array();

            for (var i = 0; i < itemsInCart.length; i++) {
                item = itemsInCart[i].split('_');

                if (item[0] === id) {
                    itemInCart = true;
                    item[1] = parseInt(item[1]) + amt;
                }
                newItemsInCart.push(item[0] + "_" + item[1]);
            }
            if (!itemInCart) {
                newItemsInCart.push(id + "_" + amt);
            }
            cart = cart + newItemsInCart.join(":");
            document.cookie = cart + expires + "; path=/";
        }
        else {
            var date = new Date();

            date.setTime(date.getTime() + (10 * 24 * 60 * 60 * 1000));//cookie lasts 10 days
            var expires = "; expires=" + date.toUTCString();
            document.cookie = "cart=" + id + "_" + amt + expires + "; path=/";
        }
    }
    read();
}

function updateItemInCart(id, amt) {
    var cookie = getCookie("cart");
    var itemsInCart = cookie.split(':');
    var newItemsInCart = new Array();
    var cart = "cart=";
    var date = new Date();
    date.setTime(date.getTime() + (10 * 24 * 60 * 60 * 1000));//cookie lasts 10 days
    var expires = "; expires=" + date.toUTCString();
    for (var i = 0; i < itemsInCart.length; i++) {
        item = itemsInCart[i].split('_');
        if (item[0] === id) {
            newItemsInCart.push(item[0] + "_" + amt);
        }
        else {
            newItemsInCart.push(item[0] + "_" + item[1]);
        }
    }
    cart = cart + newItemsInCart.join(":");
    document.cookie = cart + expires + "; path=/";
}

function removeFromCart(id) {
    var cookie = getCookie("cart");
    var itemsInCart = cookie.split(':');
    var newItemsInCart = new Array();
    for (var i = 0; i < itemsInCart.length; i++) {
        item = itemsInCart[i].split('_');
        if (item[0] !== id) {
            newItemsInCart.push(item[0] + "_" + item[1]);
        }
    }

    if (newItemsInCart.length === 0) {
        var date = new Date();
        date.setTime(date.getTime() - 1);//expire cookie
    }
    else {
        var date = new Date();
        date.setTime(date.getTime() + (10 * 24 * 60 * 60 * 1000));//cookie lasts 10 days
    }
    var expires = "; expires=" + date.toUTCString();
    document.cookie = "cart=" + newItemsInCart.join(':') + expires + "; path=/";
}

function emptyCart() {
    var date = new Date();
    date.setTime(date.getTime() - 1);//expire cookie
    var expires = "; expires=" + date.toUTCString();
    document.cookie = "cart=" + expires + "; path=/";
}