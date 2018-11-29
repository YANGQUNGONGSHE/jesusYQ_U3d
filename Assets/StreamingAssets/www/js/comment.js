
function submit () {
    document.getElementById("showLoading").classList.remove("isshow");
    var val = '';
    val = $('.comment').val();
    console.info(val)
    window.location.href = 'uniwebview://preach_comment?msg='+val;
}

function setPlaceholder(placeholder)
{
    $('.comment').attr('placeholder','回复：' + placeholder);
}

