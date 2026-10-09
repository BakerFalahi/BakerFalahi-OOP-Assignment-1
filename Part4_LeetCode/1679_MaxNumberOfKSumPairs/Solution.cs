public class Solution
{
    public int MaxOperations(int[] nums, int k)
    {
        Array.Sort(nums);

        var left = 0;
        var right = nums.Length - 1;
        var operations = 0;

        while (left < right)
        {
            var sum = nums[left] + nums[right];

            if (sum == k)
            {
                operations++;
                left++;
                right--;
            }
            else if (sum < k)
            {
                left++;
            }
            else
            {
                right--;
            }
        }

        return operations;
    }
}
