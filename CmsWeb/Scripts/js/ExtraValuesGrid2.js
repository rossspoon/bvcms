$(function () {
	$('#extragrid tbody td[id]').editable( function( sValue ) {
		var aPos = oTable.fnGetPosition( this );
		var aData = oTable.fnGetData( aPos[0] );
		aData[ aPos[1] ] = sValue;
		return sValue;
	}, { "onblur": 'submit' } );
    var oTable = $('#extragrid').dataTable({
        "iDisplayLength": -1,
        "bFilter": false,
        "bInfo": false,
        "bPaginate": false,
        "bLengthChange": false,
        "bSort": false
    });
// ReSharper disable WrongExpressionStatement
    new FixedHeader(oTable, { "left": true });
// ReSharper restore WrongExpressionStatement

});

