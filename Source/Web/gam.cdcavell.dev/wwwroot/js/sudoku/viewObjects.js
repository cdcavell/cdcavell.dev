let Sudoku = {

    // BEGIN - Game View Functions
    Game: function () {

        $(document).ready(function () {

			pageContentTransparentBackground();

			if ($("#CellFocus").val()) {
				let id = "#" + $("#CellFocus").val();
				$(id).focus();
			}

            noWait();

        });

		function SubmitValidCellValue(event) {
			let id = event.currentTarget.id;
			$("#CellFocus").val(id);

			let value = event.currentTarget.value;
			if ((value == "") || (1 <= value && value <= 9)) {
				document.updateForm.submit();
			} else {
				// If invalid, clear out value on screen 
				event.currentTarget.value = "";
			}
		}

		$(".cell").on("keyup", function (event) {
			SubmitValidCellValue(event);
		});

		$("#saveFormButton").on("click", function (event) {
			document.saveForm.submit();
		});

		$("#resetFormButton").on("click", function (event) {
			document.resetForm.submit();
		});

	},
    // END - Game View Functions

};
