#!/usr/bin/perl
use File::stat;
use Switch;
use Fcntl':mode';

$dir_name=$ARGV[0];

opendir my $dir, $dir_name or die "Cannot open dierctory $dir_name: $!";

my @file_names = readdir $dir;
closedir $dir;

@file_names=@file_names[2..$#file_names+1];

$files_count=$#file_names;
print("file_count=$files_count\n");

my @files;

for (my $i=0; $i<$files_count; $i++){
	my $mode = (stat($file_names[$i]))->mode;
        $res="";
        #printf "%-20s %o\n",$file_names[$i], $mode & 0777;        

        $mmode= $mode & 07777;
        my $mmode = sprintf "%o", $mmode;
        #printf "rgwrgwgr=%-20s", $octf;
        for (my $j=0; $j<3; $j++){
                $x=substr($mmode,$j,1);
                
                switch($x) {
                        case 0  { $res=$res."---" }
                        case 1  { $res=$res."--x" }
                        case 2  { $res=$res."-w-" }
                        case 3  { $res=$res."-wx" }
                        case 4  { $res=$res."r--" }
                        case 5  { $res=$res."r-x" }
                        case 6  { $res=$res."rw-" }
                        case 7  { $res=$res."rwx" }
                }
                #printf "\nn=%s\tres=%s\n", $x,$res;
        }

        printf "%-20s \t %-20s\n", $res,$file_names[$i];    
        push @files, [@file_names[$i], $mode_wr];
}

#my @sorted = sort { $a->[1] <=> $b->[1] } @files;
#
#$length=scalar(@sorted);
#
#printf("%-15s%-15s\n","Name","mode");
#for (my $j=0; $j < $length; $j++){
#    printf ("%-15s%-15s\n",@sorted[$j]->[0],@sorted[$j]->[1]);
#}


