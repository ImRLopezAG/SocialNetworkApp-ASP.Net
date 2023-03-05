$(document).ready(function () {
  $('#Phone').keypress(function (e) {
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
      return false;
    }
    let scope = this.value.length;
    let parenthesis = $(this).val();
    if (scope == 3 && parenthesis.indexOf('(') <= -1) {
      $(this).val('(' + parenthesis + ')' + '-');
    } else if (scope == 4 && parenthesis.indexOf('(') > -1) {
      $(this).val(parenthesis + ')-');
    } else if (scope == 5 && parenthesis.indexOf(')') > -1) {
      $(this).val(parenthesis + '-');
    } else if (scope == 9) {
      $(this).val(parenthesis + '-');
      $(this).attr('maxlength', '14');
    }
  });
});
