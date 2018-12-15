////////////////////////////////////////////////////////////////////
//                Aladdin Knowledge Systems (Deutschland) GmbH
//                    (c) AKS Ltd., All rights reserved
//
//
// $Id: haspvendorcode.cs,v 1.1 2004/03/09 16:16:06 dieter Exp $
////////////////////////////////////////////////////////////////////
using System;
using System.Text;


namespace RaidTheCageCtrl
{
    /// <summary>
    /// Class for holding the demoma vendor code.
    /// This vendor code shall be used within the sample.
    /// </summary>
    public class VendorCode
    {
        /// <summary>
        /// The AZIOK Key
        /// </summary>
        public const string vendorCode_AZIOK =
		"YLua6LGN1t+TkfxI9+RxmVVDvEM2Pmj30uNHXjb5idKKrS3uRVreh/rEfFhUujH872OaZENAds/FVUY9"+
        "XEGl9ZtrD4m7nB1tHJZMDRfU5/h3qV1UMB9yYOJLaW7REOdJK79KFHF3iLtQ54NzreEIMZdslDN/KYir"+
        "lYNagVBZCNYLHWu9q+EXVElOQATTZi0S7Al7yPyETXlDRKdmhyrZ5inLAdVYeIxskO2GsdcANQeT+o1P"+
        "sNMDKFg8biRYaLxJ095W/Cqx0rr6s7idpJoaJe0Pw8pEK7Jwx8wruZ/J019luXt90VguYstSVmrE/WUM"+
        "kPshac97t4cGzuORzgq6PwseZjw47SeSLg+KJio5vhuXfeSV8rwAC/fnUmvpC0zV1sZsJAnhePE29Jfp"+
        "o9WJVNEyZc8NL4dWKbej8ieSPP3bHpM0Kh9A+J0Aw1Kdr+T/L9+jQmygijbmYi5zqQfBctDYAFu3jkYn"+
        "xGLJzLViqcWTvvd3/ljifOdxIRRNY4IxayCrCcAXcPWYO/JP2io2O9mEbGJx9inZa6WEgUrpng3NWwdQ"+
        "GMOyqikWr9KGTUHSPSvxOVjq5Rp3mNtqoULgdvGGo2ot+Lx/bJZDNsSmd733J1jLzzad/RzNdgXHbkpw"+
        "JZO27XTUDNKIv65q4X34xOLW5n6agmB5HpXjyDmUaC30ncfIN3U+CgrNmiRSUbbBc+kJN6OJq/mu8J4S"+
        "8uaEFF8TGRJMDg==";

        /// <summary>
        /// The YWYZX Key
        /// </summary>
        public const string vendorCode_YWYZX =
		"riWwYgWmAnooGpeqmzFnLKr5q/jnb9ZHbLtsZJi3keyBBwRJElwTBWo6SqfLwydANjPDZDl2dT5dE2LP"+
        "7ro68d5qNRUvg4+A4LvSlx0wkkelDrXCH5VGQ7I80B1zq/OujUlAaK7YxZpSPpgO+zrQSkYp9LIl7t3y"+
        "6vgeissTAZbkXq9UJc1l0k64eQkp+1iMpL6RSwkbv6/Aju3jMQuTxlgCspPbjg+zNf5Kl9ra2S5y9F+I"+
        "S0w1NsnV47/G+Umni5savLXDOLw8d3vQvQDFmCztzbx0P+p3UgK+Wr6+4fhu/ZTVECsXyeYjkwLpydLP"+
        "S09caKOLfTGW1Zh2NGNzSPKm7EUim4feLCnPi5FDXyo8Yb5YXAqCLtKv04KGgymUcKXlStfbIvC/lH+Q"+
        "R2XeGiF1nQcda4rJJJnmBNIXfR05THFIdX9SHQpo7ZohwThpm02o1epqneDjuPcVRNxyA6H3zzyi2sE9"+
        "IY1Cf6hZum+5yt45Xfb42q8cF+wIBwmp5tOGjpDEuAw6ZKlYt/uN+SRR122HSyvifEdgYhsOlAk5JuG7"+
        "6mh+bDM6HO/q8ldRI48UxiipOdt/XZawjSKIolkBg/wppdQUkGgRZSkAKFBW79KOgmjhvfyzWEx+hdm7"+
        "FRwTgBU4WkFLmfNUVGLAfnEADriiIZQqDSAiahYmUSiRcy6VF4okyKJHoX/hH1MAwkgOUK/FN+9hPBq/"+
        "Rg9Pt4yHMjpHgA==";


        /// <summary>
        /// The WiseGuysKey QTZED
        /// </summary>
        public const string vendorCode_QTZED = 
        "FIEKnXam5ZSW2FnW7YamLqKk4qCoQxqFrDYhweNk3NKhqKXNbF55Ii6X0L+WP3CuYy10pHRXzkqi+8mx"+
        "pkP8i3wRybqEqCjfSxRr58MX6GyTQqgHJ5eQAcJQeMOp74dOVqS0vD0GOm1DP4PqXQmWT0KDj+k6VK+/"+
        "VbPsSai0tf3PFANke2uQlvrBbVV992tR+3Bpt8nCXSDdIo6XxDGFfAgj2jLZXL3FtfHLNiczNstPXQGi"+
        "01uMuIpF32bgNz3NvA7ZUeMHTovyYufyAssWICBRIYE0dSsqwB42EoxfxsyTJFXWSxDU6Fm8l2B2i/9n"+
        "blX1t/sK9oe9xsWQEp/0U5wlZ3PUQD6ScCMGScITj5TiajJk2k5hse4cP98bTqZ2ruWfmzAV2py+HPLP"+
        "RoD6mlhequ8AnVGznIC5Jjbyh2sOol9mxgL3kI8sM1F4/jqx2bvlR4a2Iwts8lf/pLcrBTRTI9Xg8nkC"+
        "dkcuClHD5wY3gk6RmS9S69Rosm39r9Rtw9zGWBcufRgmPIEAdcn0ff0pRV4rjMJpcjAvMvB8MCN4RNrj"+
        "gqdEqf23g271eYvj1GB4aUi9K0ZZeYL/pLljvB1iBewf37tUWK4l14A4XnY8nPC9ELF2raMX/7LelFeM"+
        "M6d3O8TJHnggHHoFF5mH+4vgiiN63emzsAW/RWxB4Reejw4KfRkfci5I1gvazlLZTDZlTZfSVWg/mvoP"+
        "CIQAY3+y+DLGbw==";
       

    

        /// <summary>
        /// Constructor - does nothing by default
        /// </summary>
        public VendorCode()
        {
        }

        /// <summary>
        /// Returns the vendor code QTZED as a byte array.
        /// </summary>
        static public byte[] Code_AZIOK
        {
            get
            {
                byte[] code = ASCIIEncoding.Default.GetBytes(vendorCode_AZIOK);
                return code;
            }
        }

        /// <summary>
        /// Returns the vendor code QTZED as a byte array.
        /// </summary>
        static public byte[] Code_YWYZX
        {
            get
            {
                byte[] code = ASCIIEncoding.Default.GetBytes(vendorCode_YWYZX);
                return code;
            }
        }

        /// <summary>
        /// Returns the vendor code QTZED as a byte array.
        /// </summary>
        static public byte[] Code_QTZED
        {
            get
            {
                byte[] code = ASCIIEncoding.Default.GetBytes(vendorCode_QTZED);
                return code;
            }
        }
         
    }
}
