HTML CSS - Visibility Hidden vs Display None
============================================

# Visibility Hidden vs Display None

The CSS property display:none means that the tag in question will not appear on the page at all 
(although you can still interact with it through the dom). 
There will be no space allocated for it between the other tags.

The CSS property visibility:hidden means that unlike display:none, 
the tag is not visible, but space is allocated for it on the page. 
The tag is rendered, it just isn't seen on the page.

# Sample Class Definitions

```CSS
.invisible {
  visibility: hidden;
}

.invisible {
  visibility: visible;
}

.hidden {
  display: none;
}

.shown {
  display: true;
}
```

