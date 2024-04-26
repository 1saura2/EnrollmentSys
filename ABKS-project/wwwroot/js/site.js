(function (window) {
	var addButtons = document.querySelectorAll('.item-add');
	var shopCart = document.querySelector('.shop-cart');

	var cartPricing = {
		cartList: {},
		priceTotal: 0,
		promoSavings: 0,
		addItem: function (id, price, quantity) {
			this.cartList[id] = { "price": +price, "quantity": quantity };
			this.updatePrice();
		},
		removeItem: function (id) {
			delete this.cartList[id];
			this.updatePrice();
		},
		updatePrice: function () {
			var sum = 0;
			for (id in this.cartList) {
				sum += this.cartList[id].price * this.cartList[id].quantity;

			} this.cartList[id]
			this.priceTotal = sum;
		},
		applyPromo: function (code) {
			var promoPrice = 0;
			// check for promocode
			if (code == "singles") {
				let item = 0;
				for (i in this.cartList) {
					if (this.cartList[i].price > item)
						item = this.cartList[i].price;
				}
				promoPrice = item / 2;
			}
			else if (code == "sale") {
				promoPrice = (this.priceTotal - this.priceTotal * .9).toFixed(2);
			}
			//Check if applied promo is bettern than current.
			if (promoPrice > this.promoSavings) {
				this.priceTotal = this.priceTotal + this.promoSavings - promoPrice;
				this.promoSavings = +promoPrice;
			}
		}
	}

	// Hide cart and keep shopping
	document.getElementById('keep-shopping').addEventListener('click', function () {
		shopCart.classList.add('hide');
		document.getElementById('display-cart').classList.remove('hide');
	});

	// display cart, if items exist
	document.getElementById('display-cart').addEventListener('click', function () {
		shopCart.classList.remove('hide');
		this.classList.add('hide');
	});

	// Promo Code application	
	document.querySelector('.cart-item-promocode').addEventListener('blur', function () {
		console.log('promo blue')
		cartPricing.applyPromo(this.value);
		if (cartPricing.promoSavings > 0) {
			this.parentNode.querySelector('.cart-item-price .price').innerHTML = cartPricing.promoSavings.toFixed(2);
			displayPriceTotal(cartPricing.priceTotal);
		}
	})

	// Add Item to Cart Functionality
	for (var i = 0; i < addButtons.length; i++) {
		addButtons[i].addEventListener('click', function () {
			//Grab item details
			var itemName = this.parentNode.querySelector('.item-name').innerHTML;
			var itemPrice = this.parentNode.querySelector('.item-price .price').innerHTML;
			var itemImage = this.parentNode.querySelector('img').getAttribute('src');

			//build and append new item to cart
			shopCart.querySelector('.shop-cart-list').appendChild(buildCartItem(itemName, itemPrice, itemImage));
			//display cart if hidden
			shopCart.classList.remove('hide');
			// update pricing
			displayPriceTotal(cartPricing.priceTotal);
		})
	}

	function buildCartItem(name, price, image) {
		// build element
		var cartItem = document.createElement('li');
		cartItem.className = 'cart-item';

		// add cart item to price list and set id
		cartPricing.addItem(Object.keys(cartPricing.cartList).length + 1, price, 1);
		cartItem.setAttribute('data-id', Object.keys(cartPricing.cartList).length);

		// Fill out cart item element
		var cartImage = "<img src='" + image + "' alt=''>";
		var cartName = "<a href='' class='cart-item-name'>" + name + "</a>";
		var cartPrice = "<div class='cart-item-price'>$<span class='price'>" + price + "</span></div>";
		cartItem.innerHTML = cartImage + cartName +
			"<input type='text' class='cart-item-quantity' placeholder='1' value='1'>" +
			cartPrice + "<button class='cart-item-remove'>X</button>";

		//apply remove event
		cartItem.querySelector('.cart-item-remove').addEventListener('click', function () {
			removeCartItem(this.parentNode);
		});

		// Update cart on change of quantity;
		cartItem.querySelector('.cart-item-quantity').addEventListener('blur', function () {
			// remove item if quantity is 0
			if (this.value == 0)
				removeCartItem(this.parentNode);
			else if (this.value) {
				// Update Pricing on quantity change
				let itemPrice = cartPricing.cartList[this.parentNode.getAttribute('data-id')].price;
				cartPricing.addItem(this.parentNode.getAttribute('data-id'), itemPrice, +this.value);
				// Display new pricing
				this.parentNode.querySelector('.cart-item-price .price').innerHTML = (itemPrice * +this.value).toFixed(2);
				displayPriceTotal(cartPricing.priceTotal);
			}
		});
		return cartItem;
	}

	function removeCartItem(node) {
		// update object
		cartPricing.removeItem(node.getAttribute('data-id'));
		// remove element
		node.remove();
		displayPriceTotal(cartPricing.priceTotal);
		// if no items in cart, hide cart
		if (Object.keys(cartPricing.cartList).length == 0)
			shopCart.classList.add('hide');
	}

	function displayPriceTotal(price) {
		document.querySelector('.cart-total-num .price').innerHTML = price.toFixed(2);
	}
})(window);