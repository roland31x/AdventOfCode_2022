require 'net/http'

def fetch(day, year = 2022)
    url = "https://adventofcode.com/#{year}/day/#{day}/input"
    uri = URI(url)
    req = Net::HTTP::Get.new(uri)

    seshfile = File.open("session.txt", "r")
    sesh = seshfile.read
    seshfile.close

    req['Cookie'] = "session=" + sesh

    res = Net::HTTP.start(uri.hostname, uri.port, use_ssl: uri.scheme == 'https') { |http|
        http.request(req)
    }

    input = res.body

    if(!File.exist?("inputs"))
        Dir.mkdir("inputs")
    end

    file = File.open("inputs/input#{day}.txt", "w")
    file.write(input)
    file.close

end

def get_input(day, year = 2022)
    if(!File.exist?("inputs/input#{day}.txt"))
        fetch(day, year)
    end

    file = File.open("inputs/input#{day}.txt", "r")
    input = file.read
    file.close
    return input
end