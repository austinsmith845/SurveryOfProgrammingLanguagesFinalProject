

use LWP::UserAgent;
use HTTP::Request;
use JSON;
use Data::Dumper;
use XML::SIMPLE;

my $user_agent = LWP::UserAgent -> new;

my $word = people;

my $request = HTTP::Request -> new (GET, 'words.bighugelabs.com/api/2/44f79606c40debda695bdcf6938cf7c6/$word/json');
