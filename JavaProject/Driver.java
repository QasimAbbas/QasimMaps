public class Driver{
	public static void main(String[] args){
		System.out.println("Welcome to the Java Map Visualizer");

		int grid[][] = new int[120][160];
		for(int i = 0; i < grid.length; i++){
			for(int j = 0; j < grid[i].length; j++){
				grid[i][j] = 1;
				System.out.print(grid[i][j]);
			}
			System.out.println();
		}


	}
}
