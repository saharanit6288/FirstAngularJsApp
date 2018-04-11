app.factory('ProductIDFactory', function () {

    var data = {
        ProductID: 0
    };

    return {
        getProductId: function () {
            return data.ProductID;
        },
        setProductId: function (productId) {
            data.ProductID = productId;
            alert(data.ProductID);
        }
    };
});