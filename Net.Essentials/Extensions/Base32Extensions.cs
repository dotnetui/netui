using System.Text;

namespace Net;

public static class Base32Extensions
{
    const string ValidChars = "QAZ2WSX3EDC4RFV5TGB6YHN7UJM8K9LP";

    public static string ToBase32(this string original)
    {
        var bytes = Encoding.ASCII.GetBytes(original);
        StringBuilder sb = new();         // holds the base32 chars
        byte index;
        int hi = 5;
        int currentByte = 0;

        while (currentByte < bytes.Length)
        {
            // do we need to use the next byte?
            if (hi > 8)
            {
                // get the last piece from the current byte, shift it to the right
                // and increment the byte counter
                index = (byte)(bytes[currentByte++] >> (hi - 5));
                if (currentByte != bytes.Length)
                {
                    // if we are not at the end, get the first piece from
                    // the next byte, clear it and shift it to the left
                    index = (byte)(((byte)(bytes[currentByte] << (16 - hi)) >> 3) | index);
                }

                hi -= 3;
            }
            else if (hi == 8)
            {
                index = (byte)(bytes[currentByte++] >> 3);
                hi -= 3;
            }
            else
            {

                // simply get the stuff from the current byte
                index = (byte)((byte)(bytes[currentByte] << (8 - hi)) >> 3);
                hi += 5;
            }

            sb.Append(ValidChars[index]);
        }

        return sb.ToString();
    }
}
