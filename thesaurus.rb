### Name: Keith Hollingsworth
### Date: 04/03/2019
### Assignment: Group Project - Thesaurus

require 'httparty'  # httparty includes the JSON library
require 'yaml'



json_info = []

def synonym(word)
    $case = ""
    from_file = YAML.load_file("synonyms.yml")
    if from_file.key?(word)
        $synonym = from_file[word]
        puts "\nWord found in YAML!\n"
    else !from_file.key?(word)
        puts "\nSearching...\n"
        url = "http://words.bighugelabs.com/api/2/a7c23f46ae2db8d879e1baf0ce2ef1cf/#{word}/json"
        response = HTTParty.get(url)
        if response.code == 404
            puts "** URL inaccessible **"
            $synonym = word
            store_synonym = {word => $synonym}
            File.open("synonyms.yml", "a") {|f| f.write(store_synonym.to_yaml.gsub("---\n", ''))}
        else
            json_info = JSON.parse(response.body)
            json_keys = json_info.keys
            json_keys.each do |k|
                if json_info[k].key?("syn")
                    $case = "has syn"
                    if word == json_info[k]["syn"][0]
                        $synonym = json_info[k]["syn"][1]
                    else
                        $synonym = json_info[k]["syn"][0]
                    end
                    break
                end
            end
            if $case != "has syn"
                json_keys.each do |k|
                    if json_info[k].key?("sim")
                        $case = "has sim"
                        $synonym = json_info[k]["sim"][0]
                        break
                    end
                end
            end
            store_synonym = {word => $synonym}
            File.open("synonyms.yml", "a") {|f| f.write(store_synonym.to_yaml.gsub("---\n", ''))}
        end
    end
    return $synonym
end

puts "\n---------\nTHESAURUS\n---------\n"

File.new("synonyms.yml", "a+")
File.open("synonyms.yml", "r+") do |f|
    first = {"" => ""}
    f.write(first.to_yaml)
end

real_file = false
while real_file == false
    puts "\nPlease enter the name of the text file:\n"
    input_file = gets.chomp
    if File.exist?(input_file)
        real_file = true
    else
        puts "\n*** The file/directory was not found or does not exist. ***\n\n"
    end
end
text_arr = []
unless File.zero?(input_file)
    File.open(input_file, "r") do |f|
        f.each_line do |line|
            line = line.downcase.gsub(/[^a-z0-9\s]/i, '')
            text_arr.push(*line.split.map(&:to_s))
        end
    end
end

text_arr = text_arr.uniq
text_arr.each_with_index do |word, i|
    if i ==0
        data = File.read(input_file)
    else
        data = $synonym_data
    end
    original = /\b#{word}\b/
    $synonym_data = data.gsub(original, synonym(word))
end

File.open(input_file, "w") do |f|
    f.write($synonym_data)
end