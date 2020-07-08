showInPopup = (url, title) => {
	$.ajax({
		type: "Get",
		url: url,
		success: function (res) {
			$("#form-modal .modal-body").html(res);
			$("#form-modal .modal-title").html(title);
			$("#form-modal").modal("show");

		}
	})
}

jQueryAjaxPost = form => {
	try {
		$.ajax({
			type: 'Post',
			url: form.action,
			data: new FormData(form),
			contentType: false,
			processData: false,
			success: function (res) {
				if (res.isValid) {
					$("#view-all").html(res.html);
					$("#form-modal .modal-body").html('');
					$("#form-modal .modal-title").html('');
					$("#form-modal").modal("hide");
				}
				else
					$("#form-modal .modal-body").html(res.html);
			},
			error: function (err) {
				console.log(err);
			}
		})
	} catch (e) {
		console.log(e);
	}

	return false;
}

jQueryAjaxDelete = form => {
	if (confirm('Ви дійсно хочете видалити цього користувача?')) {
		try {
			$.ajax({
				type: 'Post',
				url: form.action,
				data: new FormData(form),
				contentType: false,
				processData: false,
				success: function (res) {
					$("#view-all-users").html(res.html);
				},
				error: function (err) {
					console.log(err);
				}
			})
		} catch (e) {
			console.log(e);
		}
	}

	// to prevent default form submit event 
	return false;
}