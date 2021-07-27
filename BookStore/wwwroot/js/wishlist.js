function onClickWishlist()
{
    let wishlistElement = event.currentTarget;
    let wishlistID = wishlistElement.getAttribute("wishlistID");

    if (wishlistElement.getAttribute("value") === "Add to Wishlist") {
        wishlistElement.setAttribute("value", "Remove from Wishlist");
        wishlistElement.setAttribute("class", "btn btn-secondary btn-sm wishlist_buttons");
        //send this information via AJAX call in new function
        AddToWishlist(true, wishlistID);
    }

    else
    {
        wishlistElement.setAttribute("value", "Add to Wishlist");
        wishlistElement.setAttribute("class", "btn btn-outline-info btn-sm wishlist_buttons");
        //send this information via AJAX call in new function
        AddToWishlist(false, wishlistID);
    }
}

function AddToWishlist(wishlistStatus, wishlistID) {
    let xhr = new XMLHttpRequest();

    xhr.open("POST", "/Wishlist/AddToWishlist");
    xhr.setRequestHeader("Content-Type", "application/json; charset=utf8");

    xhr.onreadystatechange = function () {
        // done receiving streaming bytes
        if (this.readyState === XMLHttpRequest.DONE) {
            // check if HTTP operation is okay
            if (this.status !== 200)
                return;

            let data = JSON.parse(this.responseText);

            // if some error on server, don't update client's view
            if (!data.success)
                return;

            //let elem = document.getElementById(elemId)
            //if (!elem)
            //    return;
        }
    };

    // send wishlist data to server
    xhr.send(JSON.stringify({
        WishlistStatus: wishlistStatus,
        wishlistID: wishlistID
    }));
}